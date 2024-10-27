<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet  version = "1.0"  xmlns:xsl = "http://www.w3.org/1999/XSL/Transform"  xmlns:docs = "https://documents.ru">
	<xsl:output  method = "xml"/>
	<xsl:variable  name = "CodeCountry">,RU,BY,KZ,AM,KG,</xsl:variable>
	<xsl:template  name = "ErrorNode">
		<xsl:param  name = "MessErr"/>
		<xsl:param  name = "PathErr"/>
		<xsl:param  name = "Category"/>
		<xsl:param  name = "Num"/>
		<xsl:variable  name = "Cat">
			<xsl:choose>
				<xsl:when  test = "string-length($Category)=0">ERROR</xsl:when>
				<xsl:otherwise>
					<xsl:value-of  select = "$Category"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable  name = "name_cfg"  select = "ancestor::*/@*[local-name()='CfgName']"/>
		<RESULTINFORMATION_ITEM  xmlns:docs = "https://documents.ru">
			<ResultDescription>
				<xsl:copy-of  select = "$MessErr"/>
			</ResultDescription>
			<ResultIdDocument>
				<xsl:value-of  select = "ancestor::*[@*[local-name()='CfgName']]/@id"/>
			</ResultIdDocument>
			<ResultTypeDocument>
				<xsl:value-of  select = "ancestor::*/@*[local-name()='CfgName']"/>
			</ResultTypeDocument>
			<ResultPosInPkgDocument>
				<xsl:value-of  select = "count(ancestor::*[@*[local-name()='CfgName']]/preceding-sibling::*[@*[local-name()='CfgName']=$name_cfg])+1"/>
			</ResultPosInPkgDocument>
			<ResultPathNode>
				<xsl:value-of  select = "$PathErr"/>
			</ResultPathNode>
			<ResultCategory>
				<xsl:value-of  select = "$Cat"/>
			</ResultCategory>
			<DocumentNumber>
				<xsl:value-of  select = "$Num"/>
			</DocumentNumber>
		</RESULTINFORMATION_ITEM>
	</xsl:template>
	<xsl:template  match = "/">
		<xsl:apply-templates  select = "Package"/>
	</xsl:template>
	<!-- проверка связанности док-тов в пакете-->
	<xsl:template  match = "Package">
		<ResultInformation  xmlns:docs = "https://documents.ru">
			<xsl:if  test = "count(//WHDOCINVENTORY_ITEM)=0">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В пакете для СВХ отсутствует опись СВХ - обязательный документ.</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template  name = "flk_doc_sea"/>
		</ResultInformation>
	</xsl:template>
	<xsl:template  name = "flk_doc_sea">
		<xsl:if  test = "count(//ARRSEADECL/ARRSEADECL_ITEM)>0 and count(//CONOSAMENT/CONOSAMENT_ITEM)=0 and count(//ARRSEADECLGoods/ARRSEADECLGOODS_ITEM)=0">
			<xsl:call-template  name = "ErrorNode">
				<xsl:with-param  name = "MessErr">Пакет не содержит сведений о товарных партиях.</xsl:with-param>
				<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/RegNum</xsl:with-param>
				<xsl:with-param  name = "Num"  select = "//ARRSEADECL/ARRSEADECL_ITEM/RegNum"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:for-each  select = "//ARRSEADECL/ARRSEADECL_ITEM">
			<xsl:if  test = "string-length(LanguageCode)>0">
				<xsl:variable  name = "LgC">
					<xsl:choose>
						<xsl:when  test = "translate(LanguageCode,'QWERTYUIOPASDFGHJKLMNBVCXZ','AAAAAAAAAAAAAAAAAAAAAAAAAA')!='AA'">0</xsl:when>
						<xsl:otherwise>1</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:if  test = "$LgC=0">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Код языка документа не соответствует формату (д.б. 2 лат. бук.)</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/LanguageCode</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
			<xsl:if  test = "string-length(DeparturePort)=0">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполнено поле Название порта отправления судна.</xsl:with-param>
					<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/DeparturePort</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:if  test = "string-length(DeclarationPort)=0">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполнено поле Название порта составления декларации о грузе.</xsl:with-param>
					<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/DeclarationPort</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:if  test = "string-length(VesselName)=0">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполнено поле Наименование судна.</xsl:with-param>
					<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/VesselName</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:for-each  select = "ARRSEADECLGoods/ARRSEADECLGOODS_ITEM">
				<xsl:variable  name = "RNum"  select = "ConosamentNum"/>
				<xsl:variable  name = "RID"  select = "normalize-space(ConosamentID)"/>
				<xsl:if  test = "count(//CONOSAMENT/CONOSAMENT_ITEM[RegNum=$RNum])=0">
					<xsl:if  test = "string-length(DebarkationPort)=0">
						<xsl:call-template  name = "ErrorNode">
							<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполнено поле Наименование Порта выгрузки</xsl:with-param>
							<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/DebarkationPort</xsl:with-param>
							<xsl:with-param  name = "Num"  select = "$RNum"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if  test = "string-length(DepartureGoodsPort)=0">
						<xsl:call-template  name = "ErrorNode">
							<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполнено поле Наименование Порта отправления</xsl:with-param>
							<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/DepartureGoodsPort</xsl:with-param>
							<xsl:with-param  name = "Num"  select = "$RNum"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:if>
				<xsl:if  test = "string-length($RID)>0 and  string-length(DebarkationPortCode)=0">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполено поле Код Порта выгрузки</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/DebarkationPortCode</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length($RID)>0 and  string-length(DepartureGoodsPortCode)=0">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполено поле Код Порта отправления</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/DepartureGoodsPortCode</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GoodsDescription)=0">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполено поле Наименование груза</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/GoodsDescription</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(SupplementaryGoodsQuantity)>0 and string-length(SupplementaryMeasureUnitCode)=0">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполнено поле Код дополнительной единицы измерения</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/SupplementaryMeasureUnitCode</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(SupplementaryGoodsQuantity)=0 and (string-length(SupplementaryMeasureUnitCode)>0 or string-length(SupplementaryMeasureUnitName)>0)">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполнено поле Кол-во товара (в дополнительной единице измерения)</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/SupplementaryGoodsQuantity</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GrossWeightQuantity)=0">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна не заполено поле Вес товара, брутто (кг)</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/GrossWeightQuantity</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GrossWeightQuantity)>0">
					<xsl:call-template  name = "valid-weight">
						<xsl:with-param  name = "weight"  select = "GrossWeightQuantity"/>
						<xsl:with-param  name = "node">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/GrossWeightQuantity</xsl:with-param>
						<xsl:with-param  name = "mess">В документе Декларация о грузе при приходе/отходе судна значение в поле Вес товара, брутто (кг) не соответствует установленному формату</xsl:with-param>
						<xsl:with-param  name = "num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(TotalWeightWithCont)>0">
					<xsl:call-template  name = "valid-weight">
						<xsl:with-param  name = "weight"  select = "TotalWeightWithCont"/>
						<xsl:with-param  name = "node">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/TotalWeightWithCont</xsl:with-param>
						<xsl:with-param  name = "mess">В документе Декларация о грузе при приходе/отходе судна значение в поле Общий вес брутто с контейнером не соответствует установленному формату.</xsl:with-param>
						<xsl:with-param  name = "num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GoodsTNVEDCode)>0 and (string-length(GoodsTNVEDCode)&lt;4 or string-length(GoodsTNVEDCode)>10)">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Код товара не соответствует формату (для ПДС д.б. от 4 до 10 сим.).</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/GoodsTNVEDCode</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GoodsTNVEDCode)>0 and number(normalize-space(GoodsTNVEDCode))!=normalize-space(GoodsTNVEDCode)">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Код товара не соответствует формату (д.б. только цифры).</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/GoodsTNVEDCode</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(PlacesQuantity)>0 and contains(PlacesQuantity,',')">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Кол-во грузовых мест не соответствует формату (содержит запятую).</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/PlacesQuantity</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(PlacesQuantity)>8">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Кол-во грузовых мест не соответствует формату (более 8 сим.)</xsl:with-param>
						<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/PlacesQuantity</xsl:with-param>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:for-each  select = "Container/CONTAINER_ITEM">
					<xsl:variable  name = "ContPos"  select = "position()"/>
					<xsl:if  test = "string-length(Place)>0 and contains(Place,',')">
						<xsl:call-template  name = "ErrorNode">
							<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Кол-во мест в Контейнере не соответствует формату (содержит запятую)</xsl:with-param>
							<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/Container/CONTAINER_ITEM[<xsl:value-of  select = "position()"/>]/Place</xsl:with-param>
							<xsl:with-param  name = "Num"  select = "$RNum"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if  test = "string-length(Weight)>0">
						<xsl:call-template  name = "valid-weight">
							<xsl:with-param  name = "weight"  select = "Weight"/>
							<xsl:with-param  name = "node">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/Container/CONTAINER_ITEM[<xsl:value-of  select = "position()"/>]/Weight</xsl:with-param>
							<xsl:with-param  name = "mess">В документе Декларация о грузе при приходе/отходе судна значение  в поле Вес в Контейнере не соответствует установленному формату.</xsl:with-param>
							<xsl:with-param  name = "num"  select = "$RNum"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if  test = "string-length(WeightWithCont)>0">
						<xsl:call-template  name = "valid-weight">
							<xsl:with-param  name = "weight"  select = "WeightWithCont"/>
							<xsl:with-param  name = "node">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/Container/CONTAINER_ITEM[<xsl:value-of  select = "position()"/>]/WeightWithCont</xsl:with-param>
							<xsl:with-param  name = "mess">В документе Декларация о грузе при приходе/отходе судна значение в поле Общий вес брутто с контейнером не соответствует установленному формату.</xsl:with-param>
							<xsl:with-param  name = "num"  select = "$RNum"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:if  test = "string-length(ContainerID)>17">
						<xsl:call-template  name = "ErrorNode">
							<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле № контейнера не соответствует формату (д.б. не более 17 сим.)</xsl:with-param>
							<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/Container/CONTAINER_ITEM[<xsl:value-of  select = "position()"/>]/ContainerID</xsl:with-param>
							<xsl:with-param  name = "Num"  select = "$RNum"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
				<xsl:if  test = "count(//CONOSAMENT/CONOSAMENT_ITEM[RegNum=$RNum])=0">
					<xsl:for-each  select = "Danger/DANGER_ITEM">
						<xsl:if  test = "string-length(UNNO)>0 and string-length(normalize-space(UNNO))!=4">
							<xsl:call-template  name = "ErrorNode">
								<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Кода UNNO не соответствует формтау (д.б. 4 сим.)</xsl:with-param>
								<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/Danger/DANGER_ITEM[<xsl:value-of  select = "position()"/>]/UNNO</xsl:with-param>
								<xsl:with-param  name = "Num"  select = "$RNum"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if  test = "string-length(UNNO)>0 and not(number(translate(normalize-space(UNNO),'0123456789','0000000000'))=0)">
							<xsl:call-template  name = "ErrorNode">
								<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Кода UNNO не соответствует формтау (д.б. 4 цифры)</xsl:with-param>
								<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/Danger/DANGER_ITEM[<xsl:value-of  select = "position()"/>]/UNNO</xsl:with-param>
								<xsl:with-param  name = "Num"  select = "$RNum"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if  test = "string-length(DangerIMO)>0 and string-length(DangerIMO)>3">
							<xsl:call-template  name = "ErrorNode">
								<xsl:with-param  name = "MessErr">В документе Декларация о грузе при приходе/отходе судна значение в поле Класс опасности IMO не соответствует формтау (д.б. не более 3 сим.)</xsl:with-param>
								<xsl:with-param  name = "PathErr">ARRSEADECL/ARRSEADECL_ITEM/ARRSEADECLGoods/ARRSEADECLGOODS_ITEM[<xsl:value-of  select = "position()"/>]/Danger/DANGER_ITEM[<xsl:value-of  select = "position()"/>]/DangerIMO</xsl:with-param>
								<xsl:with-param  name = "Num"  select = "$RNum"/>
							</xsl:call-template>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
		<xsl:for-each  select = "//CONOSAMENT/CONOSAMENT_ITEM">
			<xsl:variable  name = "NameNode"  select = "CONOSAMENTGoods/*[name()='CONOSAMENTGOODS_ITEM' or name()='CONOSAMENTGoods_ITEM']"/>
			<xsl:variable  name = "Proc"  select = "Procedure"/>
			<xsl:variable  name = "ValCod"  select = "CurrencyCode"/>
			<xsl:variable  name = "Weight"  select = "number(sum($NameNode[number(GrossWeightQuantity)>0]/GrossWeightQuantity))"/>
			<xsl:variable  name = "RNum"  select = "RegNum"/>
			<xsl:if  test = "string-length(RegNum)=0">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Коносамент не заполнено поле Регистрационный номер</xsl:with-param>
					<xsl:with-param  name = "PathErr">CONOSAMENT/CONOSAMENT_ITEM/RegNum</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:if  test = "string-length(RegNum)>0 and count(//CONOSAMENT/CONOSAMENT_ITEM[RegNum=$RNum])>1">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">Пакет содержит Коносаменты с одинаковыми Регистрационными номерами.</xsl:with-param>
					<xsl:with-param  name = "PathErr">CONOSAMENT/CONOSAMENT_ITEM/RegNum</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:if  test = "string-length(DestinationOfficeIdentifier)>0 and string-length(DestinationOfficeIdentifier)!=8 and not(number(translate(DestinationOfficeIdentifier,'1234567890','0000000000'))=0)">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Коносамент значение поля Код таможенного органа назначения не соответствует формату (д.б. 8 цифр)</xsl:with-param>
					<xsl:with-param  name = "PathErr">CONOSAMENT/CONOSAMENT_ITEM/DestinationOfficeIdentifier</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$RNum"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if  test = "string-length(VesselName)=0">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Коносамент не заполнено поле Наименование судна.</xsl:with-param>
					<xsl:with-param  name = "PathErr">CONOSAMENT/CONOSAMENT_ITEM/VesselName</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$RNum"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if  test = "string-length(LoadingName)=0">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Коносамент не заполнено поле Порт погрузки: название порта.</xsl:with-param>
					<xsl:with-param  name = "PathErr">CONOSAMENT/CONOSAMENT_ITEM/LoadingName</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$RNum"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if  test = "string-length(LoadingName)>50">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Коносамент значение поля Порт погрузки: название порта не соответствует формату (д.б. не более 50 сим.) При формирования таможенного формата текст будет сокращен до 50 сим.</xsl:with-param>
					<xsl:with-param  name = "PathErr">CONOSAMENT/CONOSAMENT_ITEM/LoadingName</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$RNum"/>
					<xsl:with-param  name = "Category">WARNING</xsl:with-param>
				</xsl:call-template>
			</xsl:if>
			<xsl:if  test = "string-length(UnloadingName)=0">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">В документе Коносамент не заполнено поле Порт выгрузки: название порта.</xsl:with-param>
					<xsl:with-param  name = "PathErr">CONOSAMENT/CONOSAMENT_ITEM/UnloadingName</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$RNum"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:call-template  name = "CheckIdOrg">
				<xsl:with-param  name = "Pref">Consignor_</xsl:with-param>
				<xsl:with-param  name = "PrefMessCr">В документе Коносамент не заполнено поле Отправитель:</xsl:with-param>
				<xsl:with-param  name = "PrefMess">В документе Коносамент значение в поле Отправитель:</xsl:with-param>
				<xsl:with-param  name = "PrefPath">CONOSAMENT/CONOSAMENT_ITEM/</xsl:with-param>
				<xsl:with-param  name = "Num"  select = "$RNum"/>
			</xsl:call-template>
			<xsl:call-template  name = "CheckIdOrg">
				<xsl:with-param  name = "Pref">Consignee_</xsl:with-param>
				<xsl:with-param  name = "PrefMessCr">В документе Коносамент не заполнено поле Получатель:</xsl:with-param>
				<xsl:with-param  name = "PrefMess">В документе Коносамент значение в поле Получатель:</xsl:with-param>
				<xsl:with-param  name = "PrefPath">CONOSAMENT/CONOSAMENT_ITEM/</xsl:with-param>
				<xsl:with-param  name = "Num"  select = "$RNum"/>
			</xsl:call-template>
			<xsl:call-template  name = "CheckIdOrg">
				<xsl:with-param  name = "Pref">Carrier_</xsl:with-param>
				<xsl:with-param  name = "PrefMessCr">В документе Коносамент не заполнено поле Перевозчик:</xsl:with-param>
				<xsl:with-param  name = "PrefMess">В документе Коносамент значение в поле Перевозчик:</xsl:with-param>
				<xsl:with-param  name = "PrefPath">CONOSAMENT/CONOSAMENT_ITEM/</xsl:with-param>
				<xsl:with-param  name = "Num"  select = "$RNum"/>
			</xsl:call-template>
			<xsl:for-each  select = "$NameNode">
				<xsl:variable  name = "PosConosGoods"  select = "position()"/>
				<xsl:variable  name = "NodeErr1"  select = "name($NameNode)"/>
				<xsl:variable  name = "NodeErr"  select = "concat('CONOSAMENT/CONOSAMENT_ITEM/CONOSAMENTGoods/',$NodeErr1,'[',$PosConosGoods,']/')"/>
				<xsl:if  test = "string-length(GoodsDescription)=0">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Коносамент не заполнено поле Наименование груза.</xsl:with-param>
						<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'GoodsDescription')"/>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GoodsNomenclatureCode)>0 and (string-length(GoodsNomenclatureCode)&lt;4 or string-length(GoodsNomenclatureCode)>10)">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Коносамент значение в поле Код товара не соотвтетствует формату (для ПДС д.б. от 4 до 10 сим.).</xsl:with-param>
						<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'GoodsNomenclatureCode')"/>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GoodsNomenclatureCode)>0 and number(normalize-space(GoodsNomenclatureCode))!=normalize-space(GoodsNomenclatureCode)">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Коносамент значение в поле Код товара не соотвтетствует формату (д.б. только цифры).</xsl:with-param>
						<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'GoodsNomenclatureCode')"/>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:choose>
					<xsl:when  test = "count(Danger/DANGER_ITEM[string-length(DangerIMO)>0])>0 or count(Danger/DANGER_ITEM[string-length(UNNO)>0])>0">
						<xsl:for-each  select = "Danger/DANGER_ITEM[string-length(DangerIMO)>0]">
							<xsl:variable  name = "PDnImo"  select = "position()"/>
							<xsl:if  test = "string-length(DangerIMO)>3">
								<xsl:call-template  name = "ErrorNode">
									<xsl:with-param  name = "MessErr">В документе Коносамент значение в гр. Класс опасности не соотвтетствует формату (д.б.не более 3 сим.)</xsl:with-param>
									<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'Danger/DANGER_ITEM[',$PDnImo,']/DangerIMO')"/>
									<xsl:with-param  name = "Num"  select = "$RNum"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
						<xsl:for-each  select = "Danger/DANGER_ITEM[string-length(UNNO)>0]">
							<xsl:variable  name = "PDnImo"  select = "position()"/>
							<xsl:if  test = "string-length(UNNO)>0 and  string-length(normalize-space(UNNO))!=4">
								<xsl:call-template  name = "ErrorNode">
									<xsl:with-param  name = "MessErr">В документе Коносамент значение в гр. Идентификатор опасности вещества по классифкации ООН не соотвтетствует формату (д.б.4 сим.)</xsl:with-param>
									<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'Danger/DANGER_ITEM[',$PDnImo,']/UNNO')"/>
									<xsl:with-param  name = "Num"  select = "$RNum"/>
								</xsl:call-template>
							</xsl:if>
							<xsl:if  test = "string-length(UNNO)>0 and not(number(translate(normalize-space(UNNO),'0123456789','0000000000'))=0)">
								<xsl:call-template  name = "ErrorNode">
									<xsl:with-param  name = "MessErr">В документе Коносамент значение в гр. Идентификатор опасности вещества по классифкации ООН не соотвтетствует формату (д.б.4 цифры)</xsl:with-param>
									<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'Danger/DANGER_ITEM[',$PDnImo,']/UNNO')"/>
									<xsl:with-param  name = "Num"  select = "$RNum"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:if  test = "string-length(DangerIMO)>3">
							<xsl:call-template  name = "ErrorNode">
								<xsl:with-param  name = "MessErr">В документе Коносамент значение в поле Класс опасности не соотвтетствует формату (д.б. не более 3 сим.)</xsl:with-param>
								<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'DangerIMO')"/>
								<xsl:with-param  name = "Num"  select = "$RNum"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if  test = "string-length(UNNO)>0 and  string-length(normalize-space(UNNO))!=4">
							<xsl:call-template  name = "ErrorNode">
								<xsl:with-param  name = "MessErr">В документе Коносамент значение в поле Идентификатор опасности вещества по классифкации ООН  не соотвтетствует формату (д.б.4 сми.)</xsl:with-param>
								<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'UNNO')"/>
								<xsl:with-param  name = "Num"  select = "$RNum"/>
							</xsl:call-template>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if  test = "string-length(PlacesQuantity)>0 and contains(PlacesQuantity,',')">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Коносамент значение в поле Кол-во мест не соответствует формату (содержит запятую)</xsl:with-param>
						<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'PlacesQuantity')"/>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GrossWeightQuantity)=0">
					<xsl:call-template  name = "ErrorNode">
						<xsl:with-param  name = "MessErr">В документе Коносамент не заполнено поле Вес товара, брутто (кг).</xsl:with-param>
						<xsl:with-param  name = "PathErr"  select = "concat($NodeErr,'GrossWeightQuantity')"/>
						<xsl:with-param  name = "Num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(GrossWeightQuantity)>0">
					<xsl:call-template  name = "valid-weight">
						<xsl:with-param  name = "weight"  select = "GrossWeightQuantity"/>
						<xsl:with-param  name = "node"  select = "concat($NodeErr,'GrossWeightQuantity')"/>
						<xsl:with-param  name = "mess">В документе Коносамент значение в поле Вес товара, брутто (кг) не соответствует формату</xsl:with-param>
						<xsl:with-param  name = "mess-dop"> (д.б. до 24 цифр, до 6 знаков после запятой)</xsl:with-param>
						<xsl:with-param  name = "num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:if  test = "string-length(WeightWithCont)>0">
					<xsl:call-template  name = "valid-weight">
						<xsl:with-param  name = "weight"  select = "WeightWithCont"/>
						<xsl:with-param  name = "node"  select = "concat($NodeErr,'WeightWithCont')"/>
						<xsl:with-param  name = "mess">В документе Коносамент значение в поле Вес брутто с контейнером не соответствует формату</xsl:with-param>
						<xsl:with-param  name = "mess-dop"> (д.б. до 24 цифр, до 6 знаков после запятой)</xsl:with-param>
						<xsl:with-param  name = "num"  select = "$RNum"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template  name = "CheckIdOrg">
		<xsl:param  name = "Pref"/>
		<xsl:param  name = "PrefMess"/>
		<xsl:param  name = "PrefMessCr"/>
		<xsl:param  name = "PrefPath"/>
		<xsl:param  name = "Num"/>
		<xsl:param  name = "FlkCt"/>
		<xsl:if  test = "string-length($FlkCt)=0 and (string-length(*[name()=concat($Pref,'INN')])>0  or string-length(*[name()=concat($Pref,'OGRN')])>0 or string-length(*[name()=concat($Pref,'KPP')])>0 or string-length(*[name()=concat($Pref,'OKPOID')])>0 or string-length(*[name()=concat($Pref,'OKPO')])>0) and string-length(*[name()=concat($Pref,'CountryCode')])=0">
			<xsl:call-template  name = "ErrorNode">
				<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$PrefMessCr"/> Код страны. При формирования таможенного формата не будут созданы узлы со значениями из полей ИНН, ОГРН, ОКПО и КПП </xsl:with-param>
				<xsl:with-param  name = "PathErr"><xsl:value-of  select = "$PrefPath"/><xsl:value-of  select = "$Pref"/>CountryCode</xsl:with-param>
				<xsl:with-param  name = "Category">WARNING</xsl:with-param>
				<xsl:with-param  name = "Num"  select = "$Num"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if  test = "(string-length(*[name()=concat($Pref,'INN')])>0  or string-length(*[name()=concat($Pref,'OGRN')])>0 or string-length(*[name()=concat($Pref,'KPP')])>0 or string-length(*[name()=concat($Pref,'OKPOID')])>0 or string-length(*[name()=concat($Pref,'OKPO')])>0) and string-length(*[name()=concat($Pref,'CountryCode')])>0 and not(contains($CodeCountry,concat(',',*[name()=concat($Pref,'CountryCode')],',')))">
			<xsl:call-template  name = "ErrorNode">
				<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$PrefMess"/> Код страны не является кодом страны члена Там.союза. При формирования таможенного формата не будут созданы узлы со значениями из полей ИНН,ОГРН  и КПП </xsl:with-param>
				<xsl:with-param  name = "PathErr"><xsl:value-of  select = "$PrefPath"/><xsl:value-of  select = "$Pref"/>CountryCode</xsl:with-param>
				<xsl:with-param  name = "Category">WARNING</xsl:with-param>
				<xsl:with-param  name = "Num"  select = "$Num"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if  test = "string-length(*[name()=concat($Pref,'OGRN')])>0 and string-length(*[name()=concat($Pref,'CountryCode')])>0">
			<xsl:variable  name = "CorRu"  select = "*[name()=concat($Pref,'CountryCode')]='RU' and string-length(*[name()=concat($Pref,'OGRN')])!=13 and string-length(*[name()=concat($Pref,'OGRN')])!=15"/>
			<xsl:variable  name = "CorKz"  select = "*[name()=concat($Pref,'CountryCode')]='KZ' and string-length(*[name()=concat($Pref,'OGRN')])!=12"/>
			<xsl:if  test = "$CorRu or $CorKz">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$PrefMess"/> ОГРН не соответствует формату (для кода страны RU - д.б.13 или 15 цифр, для KZ - 12)</xsl:with-param>
					<xsl:with-param  name = "PathErr"><xsl:value-of  select = "$PrefPath"/><xsl:value-of  select = "$Pref"/>OGRN</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$Num"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
		<xsl:if  test = "string-length(*[name()=concat($Pref,'KPP')])>0 and string-length(*[name()=concat($Pref,'CountryCode')])>0 and *[name()=concat($Pref,'CountryCode')]='RU' and string-length(*[name()=concat($Pref,'KPP')])!=9">
			<xsl:call-template  name = "ErrorNode">
				<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$PrefMess"/> КПП не соответствует формату (для кода страны RU - д.б. 9 цифр)</xsl:with-param>
				<xsl:with-param  name = "PathErr"><xsl:value-of  select = "$PrefPath"/><xsl:value-of  select = "$Pref"/>KPP</xsl:with-param>
				<xsl:with-param  name = "Num"  select = "$Num"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if  test = "string-length(*[name()=concat($Pref,'OKPO')])>0 and string-length(*[name()=concat($Pref,'CountryCode')])>0">
			<xsl:variable  name = "CorRu"  select = "*[name()=concat($Pref,'CountryCode')]='RU' and string-length(*[name()=concat($Pref,'OKPO')])>10"/>
			<xsl:variable  name = "CorKz"  select = "*[name()=concat($Pref,'CountryCode')]='KG' and string-length(*[name()=concat($Pref,'OKPO')])!=8"/>
			<xsl:if  test = "$CorRu or $CorKz">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$PrefMess"/> ОКПО не соответствует формату (для кода страны RU- не более 10 сим., для KG - 8 цифр)</xsl:with-param>
					<xsl:with-param  name = "PathErr"><xsl:value-of  select = "$PrefPath"/><xsl:value-of  select = "$Pref"/>OKPO</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$Num"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
		<xsl:if  test = "string-length(*[name()=concat($Pref,'OKPOID')])>0 and string-length(*[name()=concat($Pref,'CountryCode')])>0">
			<xsl:variable  name = "CorRu"  select = "*[name()=concat($Pref,'CountryCode')]='RU' and string-length(*[name()=concat($Pref,'OKPOID')])>10"/>
			<xsl:variable  name = "CorKz"  select = "*[name()=concat($Pref,'CountryCode')]='KG' and string-length(*[name()=concat($Pref,'OKPOID')])!=8"/>
			<xsl:if  test = "$CorRu or $CorKz">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$PrefMess"/> ОКПО не соответствует формату (для кода страны RU- не более 10 сим., для KG - 8 цифр)</xsl:with-param>
					<xsl:with-param  name = "PathErr"><xsl:value-of  select = "$PrefPath"/><xsl:value-of  select = "$Pref"/>OKPOID</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$Num"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:if>
		<xsl:if  test = "string-length(*[name()=concat($Pref,'OKATO')])>0 and string-length(*[name()=concat($Pref,'OKATO')])&lt;5 or string-length(*[name()=concat($Pref,'OKATO')])>11">
			<xsl:call-template  name = "ErrorNode">
				<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$PrefMess"/> ОКАТО не соответствует формату (д.б.от 5 до 11 цифр)</xsl:with-param>
				<xsl:with-param  name = "PathErr"><xsl:value-of  select = "$PrefPath"/><xsl:value-of  select = "$Pref"/>OKATO</xsl:with-param>
				<xsl:with-param  name = "Num"  select = "$Num"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template  name = "code-customs">
		<xsl:param  name = "code"/>
		<xsl:param  name = "node"/>
		<xsl:param  name = "mess"/>
		<xsl:param  name = "flg-null"/>
		<xsl:param  name = "codetest"/>
		<xsl:if  test = "string-length($code)=0">
			<xsl:call-template  name = "ErrorNode">
				<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$mess"/> не заполнено поле Код таможенного поста</xsl:with-param>
				<xsl:with-param  name = "PathErr"  select = "$node"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if  test = "string-length($code)>3 and substring($code,string-length($code)-2,3)='000' and $code!='00000000'  and $flg-null">
			<xsl:call-template  name = "ErrorNode">
				<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$mess"/> указан Код таможни. Получателем пакета документов может быть только таможенный пост.</xsl:with-param>
				<xsl:with-param  name = "PathErr"  select = "$node"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:if  test = "string-length($code)>0 and (string-length($code)!=8 or number(translate($code,'0123456789','0000000000'))!=0)">
			<xsl:call-template  name = "ErrorNode">
				<xsl:with-param  name = "MessErr"><xsl:value-of  select = "$mess"/> значение поля Код таможенного поста не соответствует формату (д.б. 8 цифр).</xsl:with-param>
				<xsl:with-param  name = "PathErr"  select = "$node"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template  name = "valid-weight">
		<xsl:param  name = "weight"/>
		<xsl:param  name = "node"/>
		<xsl:param  name = "mess"/>
		<xsl:param  name = "mess-dop"/>
		<xsl:param  name = "num"/>
		<xsl:variable  name = "err-wh">
			<xsl:choose>
				<xsl:when  test = "contains($weight,',')">1</xsl:when>
				<xsl:when  test = "contains($weight,'.')">
					<xsl:if  test = "string-length(substring-before($weight,'.'))>18 or string-length(substring-after($weight,'.'))>6">1</xsl:if>
				</xsl:when>
				<xsl:when  test = "contains(translate($weight,'-=+;:/\|!@#$%^*()', 'AAAAAAAAAAAAAAAAA'),'A')">2</xsl:when>
				<xsl:when  test = "contains(string(number($weight)),'NaN')">2</xsl:when>
				<xsl:otherwise>
					<xsl:if  test = "string-length($weight)>24">1</xsl:if>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when  test = "$err-wh=1">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">
						<xsl:value-of  select = "concat($mess,$mess-dop)"/>
					</xsl:with-param>
					<xsl:with-param  name = "PathErr">
						<xsl:value-of  select = "$node"/>
					</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$num"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:when  test = "$err-wh=2">
				<xsl:call-template  name = "ErrorNode">
					<xsl:with-param  name = "MessErr">
						<xsl:value-of  select = "concat($mess,' (значение содержит не отображаемые символы, заполните поле заново)')"/>
					</xsl:with-param>
					<xsl:with-param  name = "PathErr">
						<xsl:value-of  select = "$node"/>
					</xsl:with-param>
					<xsl:with-param  name = "Num"  select = "$num"/>
				</xsl:call-template>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>