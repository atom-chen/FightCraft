--��ͨ�ؿ��;�Ӣ�ؿ��ɾ�
x100002_g_scriptId = 100002

--�¼�����
function x100002_OnEventFilter(param1, param2, param3, stageId, param5, param6, param7, param8, param9)

	if stageId ~= nil and param9 ~= nil and tonumber(stageId) == tonumber(param9) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100002_OnAchievementProgress(uuid, reachNum)
  	return reachNum
end

